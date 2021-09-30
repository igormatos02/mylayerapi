using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace common.sismo.enums
{
    public enum OperationalFrontType
    {
        [Description("Permissoria")]
        Permit = 1,
        [Description("Topografia")]
        Topography = 2,
        [Description("Perfuração")]
        Drilling = 3,
        [Description("Carregamento")]
        Charging = 4,
        [Description("Sismo A")]
        SeismoA = 5,
        [Description("Detonação")]
        Detonation = 6,
        [Description("Sismo B")]
        SeismoB = 7,
        [Description("Recuperação de Área")]
        Inspection = 9,
        [Description("Gravimetria")]
        Gravimetry = 10,
        [Description("Magnetometria")]
        Magnetometry = 11,
    }
}
