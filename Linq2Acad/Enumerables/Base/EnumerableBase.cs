﻿using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq2Acad
{
  public abstract class EnumerableBase<T> : IEnumerable<T> where T : DBObject
  {
    protected Database database;
    protected Transaction transaction;

    protected EnumerableBase(Database database, Transaction transaction, ObjectId containerID)
    {
      this.database = database;
      this.transaction = transaction;
      this.ID = containerID;
    }

    internal ObjectId ID { get; private set; }

    public IEnumerator<T> GetEnumerator()
    {
      var enumerable = (IEnumerable)transaction.GetObject(ID, OpenMode.ForRead);

      foreach (var item in enumerable)
      {
        yield return (T)transaction.GetObject(GetObjectID(item), OpenMode.ForRead);
      }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    protected abstract ObjectId GetObjectID(object iteratorItem);

    public abstract int Count();

    public abstract long LongCount();

    public IEnumerable<TResult> OfType<TResult>() where TResult : T
    {
      var container = (IEnumerable)transaction.GetObject(ID, OpenMode.ForRead);
      var idEnumerator = container.GetEnumerator();
      var filterType = "AcDb" + typeof(TResult).Name;

      while (idEnumerator.MoveNext())
      {
        var id = GetObjectID(idEnumerator.Current);

        // TODO: This is not enough
        if (id.ObjectClass.Name != filterType)
        {
          continue;
        }

        yield return (TResult)transaction.GetObject(id, OpenMode.ForRead);
      }
    }

    public T Item(ObjectId id)
    {
      return (T)transaction.GetObject(id, OpenMode.ForRead);
    }

    public IEnumerable<T> Items(IEnumerable<ObjectId> ids)
    {
      var table = (IEnumerable)transaction.GetObject(ID, OpenMode.ForRead);

      foreach (var id in ids)
      {
        yield return (T)transaction.GetObject(id, OpenMode.ForRead);
      }
    }

    public IdMapping Import(T item, bool replaceIfDuplicate)
    {
      return Import(new[] { item }, replaceIfDuplicate);
    }

    public IdMapping Import(IEnumerable<T> items, bool replaceIfDuplicate)
    {
      if (items.Any(i => i.Database == database))
      {
        throw new Exception("All items must be from a different database");
      }
      
      var ids = new ObjectIdCollection(items.Select(o => o.ObjectId).ToArray());
      var mapping = new IdMapping();
      var type = replaceIfDuplicate ? DuplicateRecordCloning.Replace : DuplicateRecordCloning.Ignore;
      database.WblockCloneObjects(ids, ID, mapping, type, false);
      return mapping;
    }
  }
}