// This is the development-time service worker. It intentionally does nothing so that
// `dotnet run` always serves fresh files from the network instead of a stale cache.
// The publish step swaps this file for service-worker.published.js, which is the real
// cache-first worker used offline. See https://aka.ms/blazor-offline-considerations.
self.addEventListener('fetch', () => {
});
