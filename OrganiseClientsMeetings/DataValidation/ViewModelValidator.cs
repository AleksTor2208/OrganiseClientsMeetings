using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OrganiseClientsMeetings.ViewModel;

namespace OrganiseClientsMeetings.DataValidator
{
    public class ViewModelValidator
    {
        public bool ViewModelIsInvalid(MeetingViewModel viewModel)
        {
            if (!RequiredDateNotNull(viewModel))
                return true;

            var startTime = DateTime.Parse(viewModel.StartTime);
            var endTime = DateTime.Parse(viewModel.EndTime);
            if (!IsTimePassedCorrectly(startTime, endTime))
            {
                return true;
            }
            return false;
        }

        private bool IsTimePassedCorrectly(DateTime startTime, DateTime endTime)
        {
            var isProperOrder = DateTime.Compare(startTime, endTime) < 0;
            var minTimeSpan = 5;
            var timeSpan = endTime - startTime;
            return timeSpan.TotalMinutes > minTimeSpan && isProperOrder;
        }

        private bool RequiredDateNotNull(MeetingViewModel viewModel)
        {
            return viewModel.Name != null && viewModel.Date != null && viewModel.StartTime != null
                && viewModel.EndTime != null && viewModel.Payment != null && viewModel.Address != null;
        }
    }
}