using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Search.Data
{
    public interface ILexiconProvider
    {
        long InsertWord(string stemmedWord);
        long FindWordId(string stemmedWord);
        List<long> FindAllWordIds(List<string> stemmedWords);

        List<LexiconWord> FindAllWords(int pageNumber, int pageSize, ref long totalCount);
    }
}
