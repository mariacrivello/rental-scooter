﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rental_scooter.Controllers;
using rental_scooter.Dtos;
using rental_scooter.Models;
using rental_scooter.Repositories;
using rental_scooter.Services;

namespace rental_scooters.Test
{
    public class RentalScooterTest
    {
        private readonly AppDbContext appDbContext;

        private readonly IScooterRepository scooterRepository;
        private readonly IStationRepository stationRepository;
        private readonly IRentalHistoryRepository historyRepository;


        private readonly IRentalService rentalService;

        private readonly RentalController rentalController;

        public RentalScooterTest()
        {
            DbContextOptionsBuilder dbOptions = new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString());

            appDbContext = new AppDbContext(dbOptions.Options);

            scooterRepository = new ScooterRepository(appDbContext);
            stationRepository = new StationRepository(appDbContext);
            historyRepository = new RentalHistoryRepository(appDbContext);

            rentalService = new RentalService(stationRepository, historyRepository, scooterRepository);
            rentalController = new RentalController(rentalService);
        }

        [Fact]
        public async Task Test_GetStations()
        {
            await appDbContext.Stations.AddAsync(new Station
            {
                Name = "estacion 1",
                Scooters = new List<Scooter>
                {
                    new Scooter { State = 0 }
                }
            });
            await appDbContext.SaveChangesAsync();

            var result = await rentalController.GetStations();
            var resultType = result as OkObjectResult;
            var resultList = resultType.Value as List<Station>;

            Assert.NotNull(result);
            Assert.IsType<List<Station>>(resultType.Value);
            Assert.NotNull(resultList);
            Assert.NotEmpty(resultList);
        }
    }
}
