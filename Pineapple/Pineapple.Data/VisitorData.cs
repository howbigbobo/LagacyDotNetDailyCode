﻿using System.Collections.Generic;
using System.Linq;
using Dapper;
using Pineapple.Model;

namespace Pineapple.Data
{
    public class VisitorData
    {
        public Visitor AddVisitor(Visitor visitor)
        {
            using (var cnn = SqLiteBaseRepository.DbConnection())
            {
                cnn.Open();
                visitor.Id = cnn.Query<long>(@"INSERT INTO Visitor(VisitDate) VALUES(@VisitDate);
                                             SELECT last_insert_rowid()", visitor).FirstOrDefault();
                return visitor;
            }
        }

        public List<Visitor> LoadLatestNVisitors(int latestN)
        {
            using (var cnn = SqLiteBaseRepository.DbConnection())
            {
                cnn.Open();
                return cnn.Query<Visitor>(@"SELECT ID,VisitDate FROM Visitor ORDER BY ID DESC LIMIT " + latestN).ToList();
            }
        }

        public long GetTotalVisitors()
        {
            using (var cnn = SqLiteBaseRepository.DbConnection())
            {
                cnn.Open();
                return cnn.Query<long>(@"SELECT COUNT(id) FROM Visitor").FirstOrDefault();
            }
        }
    }
}