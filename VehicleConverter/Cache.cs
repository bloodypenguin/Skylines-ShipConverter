namespace FerryConverter
{
    public static class Cache
    {
        private static VehicleInfo _metroVehicle;
        private static TransportInfo _metroTransport;
        private static VehicleInfo _tramVehicle;
        private static TransportInfo _tramTransport;
        private static VehicleInfo _ferryVehicle;
        private static TransportInfo _ferryTransport;

        public static VehicleInfo MetroVehicle
        {
            get { return _metroVehicle ?? (_metroVehicle = PrefabCollection<VehicleInfo>.FindLoaded("Metro")); }
            private set { _metroVehicle = value; }
        }

        public static TransportInfo MetroTransport
        {
            get { return _metroTransport ?? (_metroTransport = PrefabCollection<TransportInfo>.FindLoaded("Metro")); }
            private set { _metroTransport = value; }
        }

        public static VehicleInfo TramVehicle
        {
            get { return _tramVehicle ?? (_tramVehicle = PrefabCollection<VehicleInfo>.FindLoaded("Tram")); }
            private set { _tramVehicle = value; }
        }

        public static TransportInfo TramTransport
        {
            get { return _tramTransport ?? (_tramTransport = PrefabCollection<TransportInfo>.FindLoaded("Tram")); }
            private set { _tramTransport = value; }
        }

        public static VehicleInfo FerryVehicle
        {
            get { return _ferryVehicle ?? (_ferryVehicle = PrefabCollection<VehicleInfo>.FindLoaded("Ferry")); }
            private set { _ferryVehicle = value; }
        }

        public static TransportInfo FerryTransport
        {
            get { return _ferryTransport ?? (_ferryTransport = PrefabCollection<TransportInfo>.FindLoaded("Ferry")); }
            private set { _ferryTransport = value; }
        }

        public static void Reset()
        {
            MetroVehicle = null;
            MetroTransport = null;
            TramTransport = null;
            TramVehicle = null;
            FerryVehicle = null;
            FerryTransport = null;
        }
    }
}