﻿using System;
using System.Linq;
using Linq2Acad;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using AcadTestRunner;

namespace Linq2Acad.Tests
{
  public class LinetypeTableRecordAcadTests
  {
    [CommandMethod("TestCreateLinetypeTableRecord")]
    public void TestCreateLinetypeTableRecord()
    {
      var notifier = new Notification("TestCreateLinetypeTableRecord");

      try
      {
        using (var db = AcadDatabase.Active())
        {
          var newLinetype = db.Linetypes.Create("NewLinetype");
          
          var ok = Check.Table(db.Database, table => table.Has("NewLinetype"));
          if (!ok) { notifier.TestFailed("LinetypeTable does not contain an element with name 'NewLinetype'"); return; }

          ok = Check.DictionaryIDs(db.Database, ids => ids.Any(id => id == newLinetype.ObjectId));
          if (!ok) { notifier.TestFailed("LinetypeTable does not contain the newly created element"); return; }
        }
      }
      catch (System.Exception e)
      {
        notifier.TestFailed(e);
      }

      notifier.TestPassed();
    }

    [CommandMethod("TestAddLinetypeTableRecord")]
    public void TestAddLinetypeTableRecord()
    {
      var notifier = new Notification("TestCreateLinetypeTableRecord");

      try
      {
        using (var db = AcadDatabase.Active())
        {
          var newElement = new LinetypeTableRecord() { Name = "NewLinetype" };
          db.Linetypes.Add(newElement);
          
          var ok = Check.Table(db.Database, table => table.Has("NewLinetype"));
          if (!ok) { notifier.TestFailed("LinetypeTable does not contain an element with name 'NewLinetype'"); return; }
        }
      }
      catch (System.Exception e)
      {
        notifier.TestFailed(e);
      }
    
      notifier.TestPassed();
    }
  }
}
