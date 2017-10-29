#	 Author: Kai Li
#        Date: October 28, 2017
#
#        HW4, MergingMail, retrieving column name and corresponding column values
#        from .tsv files and substitute all the "tagged" values in .tmp values
#        with the relevant values being retrived from .tsv file. 
#        Note the code solution is all my personal work and does not guaranttee
#        completely error-free solution, but it does fulfil the requirement of 
#        this programming language comparison assignment. The same implementation
#        of MailMerge is being implemented in both python and c#. The first    
#        langugage promotes simplist approach (i.e. no semicolon and braces but
#        indentation) whereas the second promotes a combination of Java and C++
#        features.
                                  
#        This program shall remain as is but only for viewing purpose not 
#        for reference of any other use. Anyone attemptted to use or copy 
#        this program by any means should be completely responsible for the 
#        consequences that follow along that demeanor. 
#	 Disclaimer: I myself 
#        will not be responsible for any demage and violation of academic 
#        instition policy if such program is copied or reused. 

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
            # print("fline {}".format(fline))
            if fline < 1 :
                strToks = line.strip().split("\t")
                for word in strToks:
                    dlist[word] = ''
                    clist.append(word)
                #print(clist)
            else: 
                strToks = line.strip().split("\t")
                #if clist[0] == "NAME":
                #    dlist[clist[0]] = strToks[0] + " " + strToks[1] 
                cIter = 0; # counter for clist
                if strToks: 
                    for word in strToks: 
                        if clist:
                            if cIter < len(clist):
                                dlist[clist[cIter]] = word
                        cIter = cIter + 1
                writeToFile(dlist, tmpf)
            fline = fline + 1 # increment fline
            #print("fline : {}\n".format(fline))
            #result = "".join(str(k) + ":" + str(v) + ", " for k,v in dlist.items())
            #print(result)
            
            # "strip" removes blankspace characters from both ends of the string
            # "split" returns a list of tokens that were delimited by ',' in the 
            # original string 
            # strToks = line.strip().split(',')

            # Transform the tokens into a list of ints 
            # intValues = list(map(lambda x : int(x), strToks))

            # sorted(list) is a built-in function that takes a list and returns 
            # a new list with those elements in sorted order. The original list 
            # is not changed. 
            # for v in sorted(intValues): 
            #    output.write(str(v) + " ")
            # output.write('\n') 

# lineMap, dictionary which has column name associated with values
# tmpf, input file for matching column names
def writeToFile(lineMap, tmpf):
    filename = lineMap["ID"] + ".txt"
    print("filename:" + filename)
    with open(tmpf, 'rt') as tmpfinput, open(filename, 'w') as output:
        #deleteContent(output)
        newline = ""
        for line in tmpfinput:
            newline = line
            valueslist = re.findall(r"<<(.*?)>>", line)
            print("line: "+ line)
            print(valueslist)
            #break
            #print(valueslist)
            print('\n')
            for value in valueslist:
                print("key value:" +value)
                newline = re.sub("<<"+value+">>", lineMap[value], newline)
                print("linemap val:" + lineMap[value])
                print("newline:" +newline)
                #print(lineMap)
                print('\n')
                #print("line "+ line)
                #print("newline " + newline)
                #print("value" + keystr)
            print("newline: " + newline)
            output.write(newline)
        

# sys.argv[0] is the name of the script file 
if __name__== "__main__":
    if len(sys.argv) == 3:
        mailMerge(sys.argv[1], sys.argv[2]);
    else: 
        print("Missing argument! Your input:{}\n".format(str(sys.argv)))
