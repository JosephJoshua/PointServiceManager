﻿using PSMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PSMDesktopUI.Library.Api
{
    public class MemberEndpoint : IMemberEndpoint
    {
        private IApiHelper _apiHelper;

        public MemberEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<MemberModel>> GetAll()
        {
            await Task.Delay(2000);
            return new List<MemberModel>();

            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/member"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<MemberModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}