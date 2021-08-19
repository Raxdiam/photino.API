import { readable } from 'svelte/store';
import { Photino } from '$lib/photino';

export const photino = readable(new Photino());
