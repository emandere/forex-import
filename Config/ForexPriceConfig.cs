using System;
using AutoMapper;
using forex_import.Domain;
using forex_import.Models;
namespace forex_import.Config
{
    public class ForexPriceProfile:Profile
    {
        public ForexPriceProfile()
        {
            CreateMap<ForexPrice, ForexPriceDTO>();
            CreateMap<ForexPrice, ForexPriceMongo>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<DateTime, string>().ConvertUsing(s => s.ToString("MM/dd/yyyy HH:mm:ss"));
        }

    }
}