// Observation-only hook: it never changes or approves a tool call.
let input = '';
let oversized = false;

for await (const chunk of process.stdin) {
  input += chunk;
  if (input.length > 16_384) {
    oversized = true;
    break;
  }
}

let message = 'Audit input is unavailable.';
if (!oversized) {
  try {
    const context = JSON.parse(input);
    const event = context?.event ?? 'unknown';
    const tool = context?.tool?.name ?? 'unknown';
    message = `${event}: ${tool}`;
  } catch {
    message = 'Audit input could not be parsed.';
  }
} else {
  message = 'Audit input exceeded the size limit.';
}

process.stdout.write(JSON.stringify({ decision: 'continue', message }));
