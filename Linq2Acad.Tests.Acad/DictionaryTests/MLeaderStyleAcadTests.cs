﻿using System;
using System.Linq;
using Linq2Acad;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using AcadTestRunner;

namespace Linq2Acad.Tests
{
  public class MLeaderStyleAcadTests
  {
    [CommandMethod("TestCreateMLeaderStyle")]
    public void TestCreateMLeaderStyle()
    {
      var notifier = new Notification("TestCreateMLeaderStyle");

      try
      {
        using (var db = AcadDatabase.Active())
        {
          var newMLeaderStyle = db.MLeaderStyles.Create("NewMLeaderStyle");
          
          var ok = Check.Dictionary(db.Database, dict => dict.Contains("NewMLeaderStyle"));
          if (!ok) { notifier.TestFailed("{ContainerClassName} does not contain an element with name 'NewMLeaderStyle'"); return; }

          ok = Check.DictionaryIDs(db.Database, ids => ids.Any(id => id == newMLeaderStyle.ObjectId));
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
