using MagicVilla_VillaApi.Models;

namespace MagicVilla_VillaApi.Data 
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>{
            new VillaDTO{Id=1, Name="Hidden Leaf Village (Konohagakure)", Sqft=100, Occupancy=40000},
            new VillaDTO{Id=2, Name="Hidden Sand Village (Sunagakure)", Sqft=100, Occupancy=2000},
            new VillaDTO{Id=2, Name="Hidden Mist Village (Sunagakure)", Sqft=100, Occupancy=3000}
        };
    }
}