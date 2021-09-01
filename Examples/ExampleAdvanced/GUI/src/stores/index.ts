import { readable, writable } from 'svelte/store';
import { Photino } from 'photino.js';
import type { ViewList, ViewListItem } from '$lib/ViewList';

const storedView = localStorage.getItem('view');

export const photino = readable(new Photino());
export const viewList = writable(null as ViewList);
export const activeView = writable(null as ViewListItem);

viewList.subscribe(vl => {
  if (vl) activeView.set(vl.find((v) => v.id === storedView));
});
activeView.subscribe(v => {
  if (v) localStorage.setItem('view', v.id);
})
