﻿using HMNGasApp.WebServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMNGasApp.Services
{
    public interface IMeterReadingSoapService
    {
        Task<(bool, Installation)> GetInstallationsAsync();
        Task<(bool, MeterReadingOrder)> GetMeterReadingOrderAsync(Installation installation, MeterReadingOrder active);
        Task<(bool, string)> NewMeterReadingAsync(string reading);
        Task<(bool, MeterReadingOrder)> GetActiveMeterReadingsAsync();
        Task<(bool, List<MeterReading>)> GetMeterReadings(DateTime from, DateTime to);
    }
}