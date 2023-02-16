using System;
using System.Collections.Generic;

namespace ASPMVC.Models;

public partial class Department
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Code { get; set; }

    public Guid? ParentDepartmentId { get; set; }

    public virtual ICollection<Empoyee> Empoyees { get; } = new List<Empoyee>();

    public virtual ICollection<Department> InverseParentDepartment { get; } = new List<Department>();

    public virtual Department? ParentDepartment { get; set; }
}
