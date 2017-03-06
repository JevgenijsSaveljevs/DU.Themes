using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DU.Themes.Entities
{
    public enum RequestStatus
    {
        /// <summary>
        /// New request.
        /// <para>code: 00001</para>
        /// </summary>
        New = 1,

        /// <summary>
        /// Means Supervisor (Lecturer, Teacher) has accepted student request.
        /// <para>code: 00010</para>
        /// </summary>
        AcceptedBySupervisor = 2,

        /// <summary>
        /// Supervisor (Lecturer, Teacher) has accepted student request, 
        /// but themes should be edited by student.
        /// <para>code: 00110</para>
        /// </summary>
        NeedImprovements = 6,

        /// <summary>
        /// Supervisor (Lecturer, Teacher) has accepted student request
        /// and Themes satisfy each side.
        /// That means that <see cref="Theme"/> could be created from these request.
        /// <para>code: 01010</para>
        /// </summary>
        ThemesAccepted = 10,

        /// <summary>
        /// Request is cancelled.
        /// <para>code 10000</para>
        /// </summary>
        Cancelled = 16
    }
}
