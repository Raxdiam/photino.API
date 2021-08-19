import type { Photino } from "..";

export default abstract class APIBase {
  protected photino: Photino;

  constructor(photino: Photino) {
    this.photino = photino;
  }
}