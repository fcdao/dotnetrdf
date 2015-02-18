﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VDS.RDF.Nodes;
using VDS.RDF.Query.Engine;
using VDS.RDF.Query.Engine.Algebra;
using VDS.RDF.Query.Expressions;
using VDS.RDF.Writing.Formatting;

namespace VDS.RDF.Query.Algebra
{
    public class Extend
        : BaseUnaryAlgebra
    {
        private Extend(IAlgebra innerAlgebra, IEnumerable<KeyValuePair<String, IExpression>> assignments)
            : base(innerAlgebra)
        {
            this.Assignments = assignments.ToList().AsReadOnly();
            if (this.Assignments.Count == 0) throw new ArgumentException("Number of assignments must be >= 1", "assignments");
        }

        public static Extend Create(IAlgebra innerAlgebra, IEnumerable<KeyValuePair<String, IExpression>> assignments)
        {
            if (!(innerAlgebra is Extend)) return Wrap(innerAlgebra, assignments);

            Extend e = (Extend) innerAlgebra;
            return new Extend(e.InnerAlgebra, e.Assignments.Concat(assignments));
        }

        public static Extend Wrap(IAlgebra innerAlgebra, IEnumerable<KeyValuePair<String, IExpression>> assignments)
        {
            return new Extend(innerAlgebra, assignments);
        }

        public IList<KeyValuePair<String, IExpression>> Assignments { get; private set; }

        public override IAlgebra Copy(IAlgebra innerAlgebra)
        {
            return Create(innerAlgebra, this.Assignments);
        }

        public override void Accept(IAlgebraVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IEnumerable<ISolution> Execute(IAlgebraExecutor executor, IExecutionContext context)
        {
            return executor.Execute(this, context);
        }

        public override string ToString(IAlgebraFormatter formatter)
        {
            if (formatter == null) throw new ArgumentNullException("formatter");
            StringBuilder builder = new StringBuilder();
            builder.Append("(extend (");
            for (int i = 0; i < this.Assignments.Count; i++)
            {
                if (i > 0) builder.Append(' ');
                builder.Append('(');
                builder.Append(formatter.Format(new VariableNode(this.Assignments[i].Key)));
                builder.Append(' ');
                builder.Append(this.Assignments[i].Value.ToPrefixString(formatter));
                builder.Append(")");
            }
            builder.AppendLine(")");
            builder.AppendLineIndented(this.InnerAlgebra.ToString(formatter), 2);
            builder.AppendLine(")");
            return builder.ToString();
        }

        public override bool Equals(IAlgebra other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other == null) return false;
            if (!(other is Extend)) return false;

            Extend e = (Extend) other;
            if (this.Assignments.Count != e.Assignments.Count) return false;
            for (int i = 0; i < this.Assignments.Count; i++)
            {
                if (!this.Assignments[i].Equals(e.Assignments[i])) return false;
            }
            return this.InnerAlgebra.Equals(e.InnerAlgebra);
        }
    }
}