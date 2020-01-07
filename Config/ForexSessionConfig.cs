using System;
using AutoMapper;
using forex_import.Domain;
using forex_import.Models;
namespace forex_import.Config
{
    public class ForexSessionProfile:Profile
    {
        public ForexSessionProfile()
        {
            CreateMap<ForexSession, ForexSessionMongo>();
            CreateMap<ForexSession, ForexSessionDTO>();
            CreateMap<ForexSessionMongo, ForexSession>()
                .ForMember
                ( dest=>dest.StartDate,
                        opts=>opts.MapFrom
                        (
                            src => DateTime.Parse(src.StartDate).ToString("yyyy-MM-dd")
                        )
                )
                .ForMember
                ( dest=>dest.CurrentTime,
                        opts=>opts.MapFrom
                        (
                            src => DateTime.Parse(src.CurrentTime).ToString("yyyy-MM-dd")
                        )
                )
                .ForMember
                (dest=>dest.EndDate,
                        opts=>opts.MapFrom
                        (
                            src => DateTime.Parse(src.EndDate).ToString("yyyy-MM-dd")
                        )
                );
            CreateMap<SessionUser,SessionUserDTO>();
            CreateMap<SessionUser,SessionUserMongo>();
            CreateMap<SessionUserMongo,SessionUser>();

            CreateMap<Accounts,AccountsDTO>();
            CreateMap<Accounts,AccountsMongo>();
            CreateMap<AccountsMongo,Accounts>();

            CreateMap<Account,AccountDTO>();
            CreateMap<Account,AccountMongo>();
            CreateMap<AccountMongo,Account>();
            
            CreateMap<Strategy,StrategyDTO>();
            CreateMap<StrategyMongo,Strategy>();
            CreateMap<Strategy,StrategyMongo>();

            CreateMap<BalanceHistory,BalanceHistoryDTO>();
            CreateMap<BalanceHistory,BalanceHistoryMongo>();
            CreateMap<BalanceHistoryMongo,BalanceHistory>()
                .ForMember
                (
                   dest=>dest.Date, opts=>opts.MapFrom
                        (
                            src => DateTime.Parse(src.Date).ToString("yyyy-MM-dd")
                        )
                )    
            ;


            CreateMap<Trade,TradeDTO>();
            CreateMap<Trade,TradeMongo>();
            CreateMap<TradeMongo,Trade>().ForMember(x => x.PL, opt => opt.Ignore());

            CreateMap<Order,OrderDTO>();
            CreateMap<Order,OrderMongo>();
            CreateMap<OrderMongo,Order>();
               
        }

    }
}