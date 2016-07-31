﻿using System;
using System.Linq;
using Linq2Acad;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using AcadTestRunner;

namespace Linq2Acad.Tests
{
  public class TextStyleTableRecordAcadTests
  {
    [CommandMethod("TestCreateTextStyleTableRecord")]
    public void TestCreateTextStyleTableRecord()
    {
      var notifier = new Notification("TestCreateTextStyleTableRecord");

      try
      {
        using (var db = AcadDatabase.Active())
        {
          var newTextStyle = db.TextStyles.Create("NewTextStyle");
          
          var ok = Check.Table(db.Database, table => table.Has("NewTextStyle"));
          if (!ok) { notifier.TestFailed("TextStyleTable does not contain an element with name 'NewTextStyle'"); return; }

          ok = Check.DictionaryIDs(db.Database, ids => ids.Any(id => id == newTextStyle.ObjectId));
          if (!ok) { notifier.TestFailed("TextStyleTable does not contain the newly created element"); return; }
        }
      }
      catch (System.Exception e)
      {
        notifier.TestFailed(e);
      }

      notifier.TestPassed();
    }

    [CommandMethod("TestAddTextStyleTableRecord")]
    public void TestAddTextStyleTableRecord()
    {
      var notifier = new Notification("TestCreateTextStyleTableRecord");

      try
      {
        using (var db = AcadDatabase.Active())
        {
          var newElement = new TextStyleTableRecord() { Name = "NewTextStyle" };
          db.TextStyles.Add(newElement);
          
          var ok = Check.Table(db.Database, table => table.Has("NewTextStyle"));
          if (!ok) { notifier.TestFailed("TextStyleTable does not contain an element with name 'NewTextStyle'"); return; }
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
