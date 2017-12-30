// piglatin.cpp

#include <iostream>
#include <cstring>

using namespace std;

const int MAX = 43;
const char VOWELS[] = "aeiouy";

char* ToPigLatin(char* word);
bool isVowel(char c, bool b);

int main()
{
   // creation of 5 character strings, each length MAX
   char word[5][MAX];
   int i;       // loop counter

   cout << "Input 5 words: ";
   for (i = 0; i < 5; i++)
       cin >> word[i];

   cout << "\nPig Latin version of the 5 words:\n";
   for (i = 0; i < 3; i++)
   {
      ToPigLatin(word[i]);
      cout << word[i] << ' ';
   }
   // Note that the above outputs illustrate that the original
   //  string itself has been converted.  The outputs below illustrate
   //  that a pointer to this string is also being returned from the 
   //  function.

   cout << ToPigLatin(word[3]) << ' '
        << ToPigLatin(word[4]) << '\n';
   
   return 0;
}

// Write your definition of the ToPigLatin function here
char* ToPigLatin(char* word)
{
    // Check if word starts with vowel (excluding 'y') and
    // if it does add 'way' at the end of it
    const char* pvowel = strchr(VOWELS, tolower(word[0]));
    if (pvowel != NULL && tolower(word[0]) != 'y')
    {
        strcat(word, "way");
    }
    
    // Word does not start with a vowel
    else
    {
        // Check for capitalization and make lowercase before shifting word
        bool isCapitalized = (int) word[0] >= 65 && (int) word[0] <= 90;
        word[0] = tolower(word[0]);
        
        // Determine size of shift
        int length = strlen(word);
        int shift = strcspn(word + 1, VOWELS) + 1;
        int offset = length - shift;
        
        // Store consonants til first vowel (including 'y')
        char front[shift];
        strncpy(front, word, shift);
        
        // Shift remaining letters to begining of word
        memmove(word, word + shift, sizeof(char[offset]));
        
        // Append stored consonants
        memmove(word + offset, front, sizeof(front));
        
        // When appropriate, recapitalize word
        if (isCapitalized)
        {
            word[0] = toupper(word[0]);
        }
        
        // Append 'ay' to end of word
        strcat(word, "ay");
    }
    return word;
}