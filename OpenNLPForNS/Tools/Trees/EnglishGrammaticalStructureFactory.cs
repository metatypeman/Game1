﻿//OpenNLPForNS is based on AlexPoint/OpenNlp
//I just need OpenNLP for Net. Standard 1.6.

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNLP.Tools.Trees
{
    public class EnglishGrammaticalStructureFactory : IGrammaticalStructureFactory
    {
        /*private readonly Predicate<string> puncFilter;
        private readonly HeadFinder hf;*/

        /*public EnglishGrammaticalStructureFactory(Predicate<string> puncFilter):
            this(puncFilter, null){
          }

          public EnglishGrammaticalStructureFactory(Predicate<string> puncFilter, HeadFinder hf) {
            this.puncFilter = puncFilter;
            this.hf = hf;
          }*/

        public GrammaticalStructure NewGrammaticalStructure(Tree t)
        {
            /*if (puncFilter == null && hf == null) {*/
            return new EnglishGrammaticalStructure(t); /*
            } else if (hf == null) {
              return new EnglishGrammaticalStructure(t, puncFilter);
            } else {
              return new EnglishGrammaticalStructure(t, puncFilter, hf);
            }*/
        }
    }
}
