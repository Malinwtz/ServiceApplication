﻿using Microsoft.Azure.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectDeviceToHubFunction.Services
{
    public class AzureFunctionIotHubManager
    {
        private readonly string _connectionString = string.Empty;
        private readonly RegistryManager _registryManager;
        private readonly ServiceClient _serviceClient;

        public AzureFunctionIotHubManager(string connectionString)
        {
            _connectionString = connectionString;
            _registryManager = RegistryManager.CreateFromConnectionString(_connectionString);
            _serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
        }
        public async Task<Device> GetDeviceAsync(string deviceId)
        {
            try
            {
                var device = await _registryManager!.GetDeviceAsync(deviceId);
                if (device != null)
                    return device;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            return null!;
        }

        public async Task<Device> RegisterDeviceAsync(string deviceId)
        {
            try
            {
                var device = await _registryManager!.AddDeviceAsync(new Device(deviceId));
                if (device != null)
                    return device;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            return null!;
        }

        public string GenerateConnectionString(Device device)
        {
            try
            {
                return $"{_connectionString.Split(";")[0]};DeviceId={device.Id};SharedAccessKey={device.Authentication.SymmetricKey.PrimaryKey}";
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            return null!;
        }
    }
}
