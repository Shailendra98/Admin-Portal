using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Contexts.PickupSessionContext.Queries;
using TKW.ApplicationCore.Contexts.PickupSessionContext.DTOs;
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

        public List<MarkerDetailViewModel> Map { get; set; }
        public List<LatLng> PolylinePath { get; set; }
      
        public async Task OnGetAsync(int id, CancellationToken cancellationToken)
        {
            var model = (await _pickupSessionQueries.PickupSessionMapDetailsAsync(id, cancellationToken))
                .Where(m => m.Latitude != null && m.Longitude != null);
            Map = model
                .Select(m => new MarkerDetailViewModel {
                    Amount = m.Amount,
                    Id = m.Id,
                    Latitude = m.Latitude!.Value,
                    Longitude = m.Longitude!.Value,
                    LocationType = m.LocationType,
                    Title = m.Title,
                    Time = m.Time.ToString("hh:mm tt")
                })
                .ToList();
            
            PolylinePath = model.Where(m => m.Time != null && m.LocationType != PickupSessionMapViewModel.Type.PendingRequest && m.LocationType != PickupSessionMapViewModel.Type.RescheduledRequest && m.LocationType != PickupSessionMapViewModel.Type.CancelledRequest).OrderBy(m => m.Time).Select(m => new LatLng
            {
                Lat = m.Latitude!.Value,
                Lng = m.Longitude!.Value
            }).ToList();

            
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
        public long Id { get; set; }
        public PickupSessionMapViewModel.Type LocationType { get; set; }
        public decimal? Amount { get; set; }
        public string Title { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Time { get; set; }
    }
}
