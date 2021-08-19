import type Photino from '../Photino';
import APIBase from './APIBase';

export default class Window extends APIBase {
  constructor(photino: Photino) {
    super(photino);
  }

  setTitle(title: string): Promise<void> {
    return this.photino.send({ ns: 'window', action: 'setTitle', params: { title } });
  }

  setSize(width: number, height: number): Promise<void> {
    return this.photino.send({ ns: 'window', action: 'setSize', params: { width, height } });
  }

  maximize(): Promise<void> {
    return this.photino.send({ ns: 'window', action: 'maximize' });
  }

  minimize(): Promise<void> {
    return this.photino.send({ ns: 'window', action: 'minimize' });
  }

  restore(): Promise<void> {
    return this.photino.send({ ns: 'window', action: 'restore' });
  }

  /* show(): Promise<void> {
    return this.photino.send({ ns: 'window', action: 'show' });
  }

  hide(): Promise<void> {
    return this.photino.send({ ns: 'window', action: 'hide' });
  } */

  close(): Promise<void> {
    return this.photino.send({ ns: 'window', action: 'close' });
  }
}
