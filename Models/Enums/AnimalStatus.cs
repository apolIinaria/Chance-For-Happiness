using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChanceForHappiness.Models.Enums
{
    /// <summary>
    /// Перерахування, що визначає можливі статуси тварини в притулку.
    /// Використовується для відстеження стану тварин та керування процесом їх прихистку.
    /// </summary>
    public enum AnimalStatus
    {
        Available,
        Reserved,
        Adopted,
        InTreatment,
        Quarantine
    }
}