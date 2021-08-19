import type Photino from '../Photino';
import APIBase from './APIBase';

export default class App extends APIBase {
  constructor(photino: Photino) {
    super(photino);
  }

  exit(): Promise<void> {
    return this.photino.send({ ns: 'app', action: 'exit' });
  }
}
