// Bridges two browser-only PWA capabilities into Blazor via JSInterop:
//   - install prompt (InstallAppButton.razor)
//   - waiting-service-worker update (UpdateBanner.razor)
// Both Razor components call `subscribeInstall`/`subscribeUpdate` with a DotNetObjectReference
// once, then get called back (`OnInstallAvailable`/`OnUpdateAvailable`) whenever the browser
// fires the corresponding event — no polling needed on the .NET side.
window.algoMotionPwa = (() => {
  let deferredInstallPrompt = null;
  let waitingWorker = null;
  const installSubscribers = [];
  const updateSubscribers = [];

  window.addEventListener('beforeinstallprompt', event => {
    // Prevent the browser's own mini-infobar so InstallAppButton is the single entry point.
    event.preventDefault();
    deferredInstallPrompt = event;
    installSubscribers.forEach(dotnetRef => dotnetRef.invokeMethodAsync('OnInstallAvailable'));
  });

  window.addEventListener('appinstalled', () => {
    deferredInstallPrompt = null;
    installSubscribers.forEach(dotnetRef => dotnetRef.invokeMethodAsync('OnInstallDone'));
  });

  if ('serviceWorker' in navigator) {
    navigator.serviceWorker.register('service-worker.js')
      .then(registration => {
        const notifyIfWaiting = () => {
          if (registration.waiting) {
            waitingWorker = registration.waiting;
            updateSubscribers.forEach(dotnetRef => dotnetRef.invokeMethodAsync('OnUpdateAvailable'));
          }
        };

        registration.addEventListener('updatefound', () => {
          const installing = registration.installing;
          if (!installing) {
            return;
          }
          installing.addEventListener('statechange', () => {
            if (installing.state === 'installed' && registration.active) {
              notifyIfWaiting();
            }
          });
        });

        // Covers the case where a previous visit already left a worker waiting.
        notifyIfWaiting();
      })
      .catch(error => console.error('AlgoMotion: service worker registration failed.', error));

    let hasReloadedForUpdate = false;
    navigator.serviceWorker.addEventListener('controllerchange', () => {
      if (hasReloadedForUpdate) {
        return;
      }
      hasReloadedForUpdate = true;
      window.location.reload();
    });
  }

  return {
    subscribeInstall(dotnetRef) {
      installSubscribers.push(dotnetRef);
    },
    subscribeUpdate(dotnetRef) {
      updateSubscribers.push(dotnetRef);
    },
    isInstallAvailable() {
      return deferredInstallPrompt !== null;
    },
    async promptInstall() {
      if (!deferredInstallPrompt) {
        return false;
      }
      const event = deferredInstallPrompt;
      deferredInstallPrompt = null;
      event.prompt();
      const choice = await event.userChoice;
      return choice.outcome === 'accepted';
    },
    applyUpdate() {
      waitingWorker?.postMessage('SKIP_WAITING');
    }
  };
})();
