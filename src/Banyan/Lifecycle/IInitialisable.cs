using System.Threading.Tasks;

namespace Banyan.Lifecycle
{
    public interface IInitialisable
    {
        Task Initialise();
    }

    public interface IInitialisable<TData>
    {
        Task Initialise(TData data);
    }
}