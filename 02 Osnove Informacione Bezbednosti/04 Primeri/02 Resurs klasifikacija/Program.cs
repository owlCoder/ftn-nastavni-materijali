using Oib.Vezba02;

var resource = new ProtectedResource("rec-42", "ana", DataClassification.Confidential);
var policy = new ResourceAuthorizationService();
var owner = new AccessRequest("ana", "read", new HashSet<string>(["records:read"]));
var otherUser = new AccessRequest("marko", "read", new HashSet<string>(["records:read"]));

Console.WriteLine($"Vlasnik: {policy.CanRead(owner, resource)}");
Console.WriteLine($"Drugi korisnik: {policy.CanRead(otherUser, resource)}");

if (!policy.CanRead(owner, resource) || policy.CanRead(otherUser, resource))
    throw new InvalidOperationException("Autorizacija resursa nije dala očekivane odluke.");

