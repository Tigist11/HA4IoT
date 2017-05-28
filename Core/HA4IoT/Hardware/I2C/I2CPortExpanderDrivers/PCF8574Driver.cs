﻿using HA4IoT.Contracts.Core;
using HA4IoT.Contracts.Hardware.I2C;
using System;

namespace HA4IoT.Hardware.I2C.I2CPortExpanderDrivers
{
    public class PCF8574Driver : I2CIPortExpanderDriver
    {
        private readonly II2CBusService _i2CBus;
        private readonly I2CSlaveAddress _address;
        private readonly byte[] _buffer = new byte[1];

        public PCF8574Driver(I2CSlaveAddress address, II2CBusService i2CBus)
        {
            _address = address;
            _i2CBus = i2CBus ?? throw new ArgumentNullException(nameof(i2CBus));
        }

        public int StateSize { get; } = 1;

        public void Write(byte[] state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (state.Length != StateSize) throw new ArgumentException("Length is invalid.", nameof(state));

            _buffer[0] = state[0];
            _i2CBus.Write(_address, _buffer);
        }

        public byte[] Read()
        {
            _i2CBus.Read(_address, _buffer);
            return _buffer;
        }
    }
}