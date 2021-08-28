import { readable } from 'svelte/store';
import { Photino } from 'photino.js';

export const photino = readable(new Photino());
