import sys
import re

# "with" makes sure that the file(s) is/are properly closed at the end of the statement, 
# even when some error is raised 
# "rt" means - read mode, text mode 
# "w" means - write mode
def mailMerge(tsvf, tmpf):
    with open(tsvf, "rt") as tsvInput:
        fline = 0 # first line counter for reading col names
        dlist = {}  # record dictionary 
        clist = []  # col name list 
        for line in tsvInput:
            # if {fline < 1} read the column names to initialize dlist and clist
            if fline < 1 :
                strToks = line.strip().split("\t")
                for word in strToks:
                    dlist[word] = ''
                    clist.append(word)
            else: 
                # fline > 1, map the corresponding value(s) to column names in one record (line) 
                # delimit line by tab and strip remove blank spaces from both ends of string
                strToks = line.strip().split("\t")
                cIter = 0; # counter for clist for iteration
                if strToks: 
                    for word in strToks: 
                        if clist:
                            if cIter < len(clist):
                                dlist[clist[cIter]] = word
                        cIter = cIter + 1
                writeToFile(dlist, tmpf) # write dlist to tmp file
            fline = fline + 1 # increment fline for line counting
            

# lineMap, dictionary which has column name associated with values
# tmpf, input file for matching column names
def writeToFile(lineMap, tmpf):
    # create file name 
    filename = lineMap["ID"] + ".txt"
    with open(tmpf, 'rt') as tmpfinput, open(filename, 'w') as output:
        newline = ""
        # reading tmp file line by line
        for line in tmpfinput:
            # intitialize newline with line from tmp 
            newline = line
            # find a list of tag values inside angle brackets from line
            valueslist = re.findall(r"<<(.*?)>>", line)
            # iterate through each value in valueslist 
            for value in valueslist:
                # subtitute each <<tag>> with values from lineMap, i.e. dlist from caller
                newline = re.sub("<<"+value+">>", lineMap[value], newline)
            # write the subtituted newline to ouput file
            output.write(newline)
        print("finish writing to: " + filename);
        

# sys.argv[0] is the name of the script file 
if __name__== "__main__":
    if len(sys.argv) == 3:
        mailMerge(sys.argv[1], sys.argv[2]);
    else: 
        print("Missing argument! Your input:{}\n".format(str(sys.argv)))
