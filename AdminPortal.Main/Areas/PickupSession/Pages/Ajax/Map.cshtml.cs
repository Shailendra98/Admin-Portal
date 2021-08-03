using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.Queries.Interfaces;
using TKW.Queries.DTOs.PickupSession;
using System.Text.Json.Serialization;

namespace TKW.AdminPortal.Areas.PickupSession.Pages.Ajax
{
    
    public class MapModel : PageModel
    {
        private readonly IPickupSessionQueries _pickupSessionQueries;
        public MapModel(IPickupSessionQueries pickupSessionQueries)
        {
            _pickupSessionQueries = pickupSessionQueries;
        }

        //public List<MarkerDetailViewModel> Map { get; set; }
        public List<MarkerDetailViewModel> MapList { get; set; }
        public MapViewModel MapView { get; set; }
        public List<LatLng> PolylinePathList { get; set; }

        public async Task OnGetAsync(int id, CancellationToken cancellationToken)
        {
            MapView = await _pickupSessionQueries.PickupSessionMapDetailsAsync(id, cancellationToken);

            var Map = MapView.Map.Where(m => m.Latitude != null && m.Longitude != null).Select(m => new MarkerDetailViewModel
            {
                Amount = m.Amount,
                Id = m.Id,
                Latitude = m.Latitude!.Value,
                Longitude = m.Longitude!.Value,
                LocationType = m.LocationType,
                Title = m.Title,
                Time = m.Time.ToString("hh:mm tt")
            }).ToList();

            List<MarkerDetailViewModel> markerDetailViewModel = new List<MarkerDetailViewModel>();
            markerDetailViewModel.Add(new MarkerDetailViewModel { Latitude = MapView.StartLatitude!.Value, Longitude = MapView.StartLongitude!.Value,LocationType = PickupSessionMapViewModel.Type.PickupSessionStart });

            List<MarkerDetailViewModel> markerDetailView = new List<MarkerDetailViewModel>();
            markerDetailView.Add(new MarkerDetailViewModel { Latitude = MapView.EndLatitude!.Value, Longitude = MapView.EndLongitude!.Value,LocationType = PickupSessionMapViewModel.Type.PickupSessionEnd });

            MapList = Map.Concat(markerDetailView).Concat(markerDetailViewModel).ToList();



            List<LatLng> pickupSessionStart = new List<LatLng>();
            pickupSessionStart.Add(new LatLng { Lat = MapView.StartLatitude.Value, Lng = MapView.StartLongitude.Value });

            List<LatLng> pickupSessionEnd = new List<LatLng>();
            pickupSessionEnd.Add(new LatLng { Lat = MapView.EndLatitude.Value, Lng = MapView.EndLongitude.Value });

            var PolylinePath = MapView.Map.Where(m => m.Time != null && m.LocationType != PickupSessionMapViewModel.Type.PendingRequest && m.LocationType != PickupSessionMapViewModel.Type.RescheduledRequest && m.LocationType != PickupSessionMapViewModel.Type.CancelledRequest).OrderBy(m => m.Time).Select(m => new LatLng
            {
                Lat = m.Latitude!.Value,
                Lng = m.Longitude!.Value
            }).ToList();

            PolylinePathList = PolylinePath.Concat(pickupSessionStart).Concat(pickupSessionEnd).ToList();

        }
    }
    public class LatLng
    {
        [JsonPropertyName("lat")]
        public decimal Lat { get; set; }

        [JsonPropertyName("lng")]
        public decimal Lng { get; set; }
    }
    public class MarkerDetailViewModel
    {
        public long? Id { get; set; }
        public PickupSessionMapViewModel.Type LocationType { get; set; }
        public decimal? Amount { get; set; }
        public string? Title { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? Time { get; set; }
    }
}
