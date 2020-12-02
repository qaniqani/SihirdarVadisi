using System;
using System.Collections.Generic;
using AdminProject.Services.Models;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Services.Interface
{
    public interface IEsportCalendarService : IBaseInterface<EsportCalendar>
    {
        IList<EsportCalendar> List(StatusTypes status);
        IList<EsportCalendarDto> List(DateTime startDate);
    }
}