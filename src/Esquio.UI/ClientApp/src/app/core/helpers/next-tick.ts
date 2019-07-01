/**
 * Awaits to the new cycle of the ui
 */
export function nextTick(time = 10) {
    return new Promise(resolve => {
        setTimeout(resolve, time);
    });
}
