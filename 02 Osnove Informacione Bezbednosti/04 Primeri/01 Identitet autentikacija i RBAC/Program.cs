using Oib.Vezba01;

var operatorActor = new Actor("ana", true, new HashSet<string>(["Operator"]));
var authorizer = new RbacAuthorizer(new RoleCatalog());

var view = authorizer.Authorize(operatorActor, "reports:view");
var export = authorizer.Authorize(operatorActor, "reports:export");

Console.WriteLine($"PREGLED: {view.Allowed} — {view.Reason}");
Console.WriteLine($"IZVOZ:   {export.Allowed} — {export.Reason}");

if (!view.Allowed || export.Allowed) throw new InvalidOperationException("RBAC primer nije dao očekivane odluke.");

