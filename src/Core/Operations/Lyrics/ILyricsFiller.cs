using System.Threading.Tasks;
using Core.FileSystem;
using LanguageExt;

namespace Core.Operations.Lyrics
{
    public interface ILyricsFiller
    {
        Task<Unit> FillLyrics(IFileElement file);
    }
}
