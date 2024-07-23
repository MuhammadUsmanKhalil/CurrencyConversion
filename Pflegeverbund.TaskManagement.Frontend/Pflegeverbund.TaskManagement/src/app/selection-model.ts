import { SelectionChange } from "@angular/cdk/collections";

export declare class SelectionModel<T> {
    private _multiple;
    private _emitChanges;
    private _selected;
    readonly selected: T[];
    readonly changed: import("rxjs").Observable<void>;
    readonly selectionChange: import("rxjs").Observable<SelectionChange<T>>;
    private _selection;
    constructor(_multiple?: boolean, initiallySelected?: T[], _emitChanges?: boolean);
    readonly isEmpty: boolean;
    readonly hasValue: boolean;
    toggle(data: T): void;
    select(...values: T[]): void;
    deselect(...values: T[]): void;
    selectAll(...values: T[]): void;
    clear(): void;
    isSelected(value: T): boolean;
    private _emitChangeEvent;
}
