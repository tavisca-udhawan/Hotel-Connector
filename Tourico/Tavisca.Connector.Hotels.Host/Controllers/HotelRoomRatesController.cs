﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tavisca.Connector.Hotels.Common;
using Tavisca.Connector.Hotels.ErrorHandling;
using Tavisca.Connector.Hotels.Model.RoomRates;
using Tavisca.Connector.Hotels.WebAPI;
using Tavisca.Platform.Common.Profiling;
using static Tavisca.Connector.Hotels.Host.Constants;

namespace Tavisca.Connector.Hotels.Host.Controllers
{
    [Route(WebApiRoute.BaseRoute)]
    public class HotelRoomRatesController : Controller
    {
        private readonly IHotelRoomRates _hotelRoomRates;

        public HotelRoomRatesController(IHotelRoomRates hotelRoomRates)
        {
            _hotelRoomRates = hotelRoomRates;
        }

        [HttpPost, Route(WebApiRoute.RoomRatesRoute)]
        public async Task<IActionResult> RoomRatesHotel([FromBody] Request request)
        {
            using (var profileScope = new ProfileContext("HotelRoomRatesController.RoomRatesHotel", false))
            {
                var validationErrors = ModalValidator.GetRequestFormatErrors(ModelState);
                if (validationErrors != null && validationErrors.Count > 0)
                {
                    var error = new ErrorInfo(FaultCodes.ValidationFailure, ErrorMessages.ValidationFailure(), System.Net.HttpStatusCode.BadRequest, validationErrors);
                    return BadRequest(error);
                }
                CallContext.SetSessionId(request?.SessionId);
                CallContext.SetSupplierId(request?.Supplier?.Id);
                CallContext.SetSupplierName(request?.Supplier?.Name);
                var response = await _hotelRoomRates.GetRoomRatesAsync(request);
                return Ok(response);
            }
        }
    }
}
