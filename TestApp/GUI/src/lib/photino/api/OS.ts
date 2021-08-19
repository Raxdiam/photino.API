import type { Photino } from '..';
import APIBase from './APIBase';

export interface DialogFilter {
  name: string;
  patterns: string[];
}

export interface DialogResult {
  success: boolean;
  path: string;
  paths: string[];
}

export default class OS extends APIBase {
  constructor(photino: Photino) {
    super(photino);
  }

  joinPaths(...paths: string[]): Promise<string> {
    return this.photino.send({ ns: 'os', action: 'joinPaths', params: { paths } });
  }

  cmd(command: string): Promise<string> {
    return this.photino.send({ ns: 'os', action: 'cmd', params: { command } });
  }

  showOpenFileDialog(title: string, multiselect: boolean, ...filters: DialogFilter[]): Promise<DialogResult> {
    return this.photino.send({ ns: 'os', action: 'showOpenFileDialog', params: { title, multiselect, filters } });
  }

  showOpenFolderDialog(title: string): Promise<DialogResult> {
    return this.photino.send({ ns: 'os', action: 'showOpenFolderDialog', params: { title } });
  }

  showSaveFileDialog(title: string, ...filters: DialogFilter[]): Promise<DialogResult> {
    return this.photino.send({ ns: 'os', action: 'showSaveFileDialog', params: { title, filters } });
  }
}
