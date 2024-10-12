﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GestionPrestamos.Models;

public partial class CobrosDetalle
{
    [Key]
    public int DetalleId { get; set; }

    public int CobroId { get; set; }

    public int PrestamoId { get; set; }

    public double ValorCobrado { get; set; }

    /*
     Foreign Key: The CobroId property in CobrosDetalle is a foreign key that links to the Cobros entity.
     Inverse Property: The Cobro property in CobrosDetalle corresponds to the CobroDetalle collection in the Cobros class, establishing a bidirectional relationship.
     Navigation Property: The Cobro property allows navigation from a CobrosDetalle entity to its related Cobros entity, facilitating access to related data.
     */
    [ForeignKey("CobroId")]
    [InverseProperty("CobroDetalle")]
    public virtual Cobros Cobro { get; set; } = null!;
}
