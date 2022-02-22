using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Linq2Acad
{
  /// <summary>
  /// A container class that provides access to the elements of the TableStyle ditionary.
  /// </summary>
  public sealed class TableStyleContainer : DBDictionaryEnumerable<TableStyle>
  {
    internal TableStyleContainer(Database database, Transaction transaction, ObjectId containerID)
      : base(database, transaction, containerID, s => s.Name, () => nameof(TableStyle.Name))
    {
    }
  }
}
