import type { SvelteComponent } from 'svelte';
import type ViewItem from '$components/ViewItem.svelte';

type ViewListItem = {
  id: string;
  name: string;
  iconClass: string;
  component: typeof SvelteComponent;
  item?: ViewItem;
};
type ViewList = ViewListItem[];
export type { ViewList, ViewListItem };
