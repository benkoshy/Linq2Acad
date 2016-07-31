﻿using System;
using System.Linq;
using Linq2Acad;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using AcadTestRunner;

namespace Linq2Acad.Tests
{
  public class PlotSettingsAcadTests
  {
    [CommandMethod("TestCreatePlotSettings")]
    public void TestCreatePlotSettings()
    {
      var notifier = new Notification("TestCreatePlotSettings");

      try
      {
        using (var db = AcadDatabase.Active())
        {
          var newPlotSettings = db.PlotSettings.Create("NewPlotSettings", true);
          
          var ok = Check.Dictionary(db.Database, dict => dict.Contains("NewPlotSettings"));
          if (!ok) { notifier.TestFailed("{ContainerClassName} does not contain an element with name 'NewPlotSettings'"); return; }

          ok = Check.DictionaryIDs(db.Database, ids => ids.Any(id => id == newPlotSettings.ObjectId));
          if (!ok) { notifier.TestFailed("{ContainerClassName} does not contain the newly created element"); return; }
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
