import { readable } from 'svelte/store';
import { Photino } from 'photino-api';

export const photino = readable(new Photino());
