using FiapTechChallenge.Domain.Entities;
using FiapTechChallenge.Infra.Data;
using FiapTechChallenge.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FiapTechChallenge.Infra.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly IServiceProvider _serviceProvider;

        public DbInitializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Initialize()
        {
            using (var context = new AppDbContext(_serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                context.Database.EnsureCreated();

                if (context.Regions.Any())
                {
                    return;
                }

                context.Regions.Add(new Region
                {
                    RegionName = "Centro-Oeste",
                    States = new List<State>
                    {
                        new State()
                        {
                            StateName = "Distrito Federal",
                            UF = "DF",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 61,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        },
                        new State()
                        {
                            StateName = "Goiás",
                            UF = "GO",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 61,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 62,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 64,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        },
                        new State()
                        {
                            StateName = "Mato Grosso",
                            UF = "MT",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 65,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 66,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        },
                        new State()
                        {
                            StateName = "Mato Grosso do Sul",
                            UF = "MS",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 67,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        }

                    },
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                });

                context.Regions.Add(new Region
                {
                    RegionName = "Nordeste",
                    States = new List<State>
                    {
                        new State()
                        {
                            StateName = "Alagoas",
                            UF = "AL",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 82,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        },
                        new State()
                        {
                            StateName = "Bahia",
                            UF = "BA",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 71,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 73,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 74,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 75,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 77,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                            },
                             Created = DateTime.Now,
                             Modified = DateTime.Now
                        },
                        new State()
                        {
                            StateName = "Ceará",
                            UF = "CE",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 85,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 88,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        },
                        new State()
                        {
                            StateName = "Maranhão",
                            UF = "MA",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 98,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 99,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        },
                        new State()
                        {
                            StateName = "Paraíba",
                            UF = "PB",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 83,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        },
                        new State()
                        {
                            StateName = "Pernambuco",
                            UF = "PE",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 81,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 87,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        },
                        new State()
                        {
                            StateName = "Piauí",
                            UF = "PI",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 86,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 89,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        },
                        new State()
                        {
                            StateName = "Rio Grande do Norte",
                            UF = "RN",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 84,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        },
                        new State()
                        {
                            StateName = "Sergipe",
                            UF = "SE",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 79,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        }
                    },
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                });

                context.Regions.Add(new Region
                {
                    RegionName = "Norte ",
                    States = new List<State>
                    {
                        new State()
                        {
                            StateName = "Acre",
                            UF = "AC",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 68,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        },
                        new State()
                        {
                            StateName = "Amapá",
                            UF = "AP",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 96,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        },
                        new State()
                        {
                            StateName = "Amazonas",
                            UF = "AM",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 92,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 97,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        },
                        new State()
                        {
                            StateName = "Pará",
                            UF = "PA",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 91,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 93,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 94,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        },
                        new State()
                        {
                            StateName = "Rondônia",
                            UF = "RO",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 69,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        },
                        new State()
                        {
                            StateName = "Roraima",
                            UF = "RR",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 95,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        },
                        new State()
                        {
                            StateName = "Tocantins",
                            UF = "TO",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 63,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        }
                    },
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                });

                context.Regions.Add(new Region
                {
                    RegionName = "Sudeste",
                    States = new List<State>
                    {
                        new State()
                        {
                            StateName = "São Paulo",
                            UF = "SP",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 11,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                     DDDNumber = 12,
                                     Created = DateTime.Now,
                                     Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                     DDDNumber = 13,
                                     Created = DateTime.Now,
                                     Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                     DDDNumber = 14,
                                     Created = DateTime.Now,
                                     Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                     DDDNumber = 15,
                                     Created = DateTime.Now,
                                     Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                     DDDNumber = 16,
                                     Created = DateTime.Now,
                                     Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                     DDDNumber = 17,
                                     Created = DateTime.Now,
                                     Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                     DDDNumber = 18,
                                     Created = DateTime.Now,
                                     Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                     DDDNumber = 19,
                                     Created = DateTime.Now,
                                     Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        },
                        new State()
                        {
                            StateName = "Rio de Janeiro",
                            UF = "RJ",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 21,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                     DDDNumber = 22,
                                     Created = DateTime.Now,
                                     Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                     DDDNumber = 24,
                                     Created = DateTime.Now,
                                     Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        },
                        new State()
                        {
                            StateName = "Espírito Santo",
                            UF = "ES",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 27,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                     DDDNumber = 28,
                                     Created = DateTime.Now,
                                     Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        },
                        new State()
                        {
                            StateName = "Minas Gerais",
                            UF = "MG",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 31,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                     DDDNumber = 32,
                                     Created = DateTime.Now,
                                     Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                     DDDNumber = 33,
                                     Created = DateTime.Now,
                                     Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                     DDDNumber = 34,
                                     Created = DateTime.Now,
                                     Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                     DDDNumber = 35,
                                     Created = DateTime.Now,
                                     Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                     DDDNumber = 37,
                                     Created = DateTime.Now,
                                     Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                     DDDNumber = 38,
                                     Created = DateTime.Now,
                                     Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        }
                    },
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                });

                context.Regions.Add(new Region
                {
                    RegionName = "Sul",
                    States = new List<State>
                    {
                        new State()
                        {
                            StateName = "Paraná",
                            UF = "PR",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 41,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 42,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 43,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 44,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 45,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 46,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 49,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        },
                        new State()
                        {
                            StateName = "Rio Grande do Sul",
                            UF = "RS",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 51,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 53,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 54,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 55,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        },
                        new State()
                        {
                            StateName = "Santa Catarina",
                            UF = "SC",
                            DDDs = new List<DDD>()
                            {
                                new DDD()
                                {
                                    DDDNumber = 42,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 47,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 48,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                },
                                new DDD()
                                {
                                    DDDNumber = 49,
                                    Created = DateTime.Now,
                                    Modified = DateTime.Now
                                }
                            },
                            Created = DateTime.Now,
                            Modified = DateTime.Now
                        }
                    },
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                });

                context.PhoneTypes.Add(new PhoneType
                {   
                    Description = "Residencial",
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                });
                
                context.PhoneTypes.Add(new PhoneType
                {
                    Description = "Celular",
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                });
                
                context.PhoneTypes.Add(new PhoneType
                {
                    Description = "Comercial",
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                });

                
                context.SaveChanges();
            }
        }
    }
}
