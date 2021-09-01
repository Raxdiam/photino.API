import type { SvelteComponent } from 'svelte';
import type { ActionItem } from '$components';

type ActionListItem = {
  id: string;
  name: string;
  iconClass: string;
  run: () => void;
  item?: ActionItem;
};
type ActionList = ActionListItem[];

export type { ActionListItem, ActionList };
