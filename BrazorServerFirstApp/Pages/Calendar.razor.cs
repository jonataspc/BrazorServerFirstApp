using BrazorServerFirstApp.Models;
using Radzen;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BrazorServerFirstApp.Pages
{
    public partial class Calendar
    {

        string text = "Click me!";

        private void ButtonClicked()
        {
            text = $"Clicked at {DateTime.Now:F}";
        }


        RadzenScheduler<Appointment> scheduler;
        //EventConsole console;
        Dictionary<DateTime, string> events = new Dictionary<DateTime, string>();

        IList<Appointment> appointments = new List<Appointment>
    {
    new Appointment { Start = DateTime.Today.AddDays(-2), End = DateTime.Today.AddDays(-2), Text = "Birthday" },
    new Appointment { Start = DateTime.Today.AddDays(-11), End = DateTime.Today.AddDays(-10), Text = "Day off" },
    new Appointment { Start = DateTime.Today.AddDays(-10), End = DateTime.Today.AddDays(-8), Text = "Work from home" },
    new Appointment { Start = DateTime.Today.AddHours(10), End = DateTime.Today.AddHours(12), Text = "Online meeting" },
    new Appointment { Start = DateTime.Today.AddHours(10), End = DateTime.Today.AddHours(13), Text = "Skype call" },
    new Appointment { Start = DateTime.Today.AddHours(14), End = DateTime.Today.AddHours(14).AddMinutes(30), Text = "Dentist appointment" },
    new Appointment { Start = DateTime.Today.AddDays(1), End = DateTime.Today.AddDays(12), Text = "Vacation" },
    };

        async Task OnSlotSelect(SchedulerSlotSelectEventArgs args)
        {
            //console.Log($"SlotSelect: Start={args.Start} End={args.End}");

            Appointment data = await DialogService.OpenAsync<AddAppointmentPage>("Add Appointment",
                new Dictionary<string, object> { { "Start", args.Start }, { "End", args.End } });

            if (data != null)
            {
                appointments.Add(data);
                // Either call the Reload method or reassign the Data property of the Scheduler
                await scheduler.Reload();
            }
        }

        public async Task OnAppointmentSelect(SchedulerAppointmentSelectEventArgs<Appointment> args)
        {
            //console.Log($"AppointmentSelect: Appointment={args.Data.Text}");

            Appointment data = await DialogService.OpenAsync<EditAppointmentPage>("Edit Appointment", new Dictionary<string, object> { { "Appointment", args.Data } });

            if (data != null)
            {
                appointments.Remove(data);
            }

            await scheduler.Reload();
        }

        public void OnAppointmentRender(SchedulerAppointmentRenderEventArgs<Appointment> args)
        {
            // Never call StateHasChanged in AppointmentRender - would lead to infinite loop

            if (args.Data.Text == "Birthday")
            {
                args.Attributes["style"] = "background: red";
            }
        }
    }
}
