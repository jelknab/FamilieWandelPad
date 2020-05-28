using System.Threading.Tasks;

namespace FamilieWandelPad
{
    public interface ILocationPermission
    {
        Task CheckAndAsk();
    }
}