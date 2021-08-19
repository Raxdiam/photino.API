import type { Photino } from '..';
import APIBase from './APIBase';

export default class IO extends APIBase {
  constructor(photino: Photino) {
    super(photino);
  }

  readFile(path: string, encoding: BufferEncoding | null = null): Promise<string> {
    return this.photino.send({ ns: 'io', action: 'readFile', params: { path, encoding } });
  }

  writeFile(path: string, contents: string, encoding: BufferEncoding | null = null): Promise<void> {
    return this.photino.send({ ns: 'io', action: 'writeFile', params: { path, contents, encoding } });
  }

  listFiles(path: string, searchPattern: string = null, recursive: boolean = false): Promise<string[]> {
    return this.photino.send({ ns: 'io', action: 'listFiles', params: { path, searchPattern, recursive } });
  }

  listFolders(path: string, searchPattern: string = null, recursive: boolean = false): Promise<string[]> {
    return this.photino.send({ ns: 'io', action: 'listFolders', params: { path, searchPattern, recursive } });
  }

  createFolder(path: string): Promise<void> {
    return this.photino.send({ ns: 'io', action: 'createFolder', params: { path } });
  }

  deleteFile(path: string): Promise<void> {
    return this.photino.send({ ns: 'io', action: 'deleteFile', params: { path } });
  }

  deleteFolder(path: string, recursive: boolean = false): Promise<void> {
    return this.photino.send({ ns: 'io', action: 'deleteFolder', params: { path, recursive } });
  }

  cwd(): Promise<string> {
    return this.photino.send({ ns: 'io', action: 'cwd', params: {} });
  }
}
